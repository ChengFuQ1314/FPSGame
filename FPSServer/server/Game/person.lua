local skynet=require "skynet"
local socket = require "skynet.socket"
require "skynet.manager"


local CMD={}
local master
local mysqldb
local persononline=false
local response


local _cID,_Accountnumber,_addr,_name,_level,_experience,_kills,_deaths

function DataHandler(data)
    local _start_num=string.find(data,"@")
    local _level_num=string.find(data,"/")
    _level=string.sub(data,_start_num+1,_level_num-1)

    data=string.sub(data,_level_num+1)
    local _experience_num=string.find(data,"/")
    _experience=string.sub(data,0,_experience_num-1)
    
    data=string.sub(data,_experience_num+1)
    local _kills_num=string.find(data,"/")
    _kills=string.sub(data,0,_kills_num-1)

    data=string.sub(data,_kills_num+1)
    local _deaths_num=string.find(data,"/")
    _deaths=string.sub(data,0,_deaths_num-1)
end

function LevelUpdata()
    if tonumber(_experience)<=0 then
        return
    end
    if tonumber(_experience)%100==0 then
        _level=_level+1;
    end
end

function CMD.GAMEDATA(data)
    DataHandler(data)
    LevelUpdata()
    skynet.send(".redis","lua","updateClientData",_Accountnumber,_level,_experience,_kills,_deaths)
end


function CMD.INIT(m)
    master=m
    mysqldb=skynet.uniqueservice("mySql")
end

function CMD.ONLINE(cID,addr,name,accountnumber)
    if persononline then
        return "#"
    end
    _Accountnumber=accountnumber
    _cID=cID
    _addr=addr
    _name=name
    persononline=true
    skynet.error("ID:".._cID,"Addr:".._addr,"Name:".._name,"Person:"..skynet.self())
    skynet.send(".chat","lua","chatJoin",_cID,_name)
    return "@",skynet.self()
end

function CMD.DISCONNECT(fd)
    if fd==_cID then
        skynet.send(".redis","lua","deleteClient",_Accountnumber,_level,_experience,_kills,_deaths)
        persononline=false
        _cID=nil
        _addr=nil
        _name=nil
        _level=0
        _deaths=0
        _kills=0
        _Accountnumber=nil
        _experience=0
    end
end

skynet.start(function ()
    _experience=0
    _level=0
    _deaths=0
    _kills=0
    skynet.register(".person")
    skynet.dispatch("lua",function (session,address,command,...)
        response=skynet.response(skynet.pack)
        command=command:upper()
        local func=assert(CMD[command])
        if func then
            response(true,func(...))
        else
            skynet.error("Person Command Error")
        end
    end)
end)