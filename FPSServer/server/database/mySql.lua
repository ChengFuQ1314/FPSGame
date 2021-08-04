local skynet=require "skynet"
local mysql=require "skynet.db.mysql"
local manager=require "skynet.manager"

local mydb={}
local data={}
local tdb
local value=nil

function mydb.on_connect()
    --
end

function mydb.databaseConnect()
    tdb=mysql.connect({
        host="127.0.0.1",
        port=3306,
        database="FPS_Game",
        user="root",
        password="jkv18w1204",
        max_packet_size=1024*1024,
        on_connect=mydb.on_connect
    })
    if not tdb then
        skynet.error("failed to connect")
    else
        skynet.error("success to connect")
    end
end

function mydb.disConnect()
    tdb:disconnect()
    skynet.error("-------------Mysql End-------------")
end


function mydb.dump(res,tab)
    local num=1
    tab = tab or 0
    if type(res) == "table" then
        for k,v in pairs(res) do
            if type(v)=="table" then
                local ret=mydb.dump(v,tab+1)
                if ret=="@" then
                    return "@"
                end
            else
                value=v
                return "@"
            end
            
        end
    end
    return "#"
end

function mydb.GETDATA(loginID)
    local res={}

    res=tdb:query("select Level from userMsg,userData where userMsg.loginID=userData.ID and userData.ID='"..loginID.."'")
    mydb.dump(res)
    data['level']=value

    res=tdb:query("select Experience from userMsg,userData where userMsg.loginID=userData.ID and userData.ID='"..loginID.."'")
    mydb.dump(res)
    data['experience']=value

    res=tdb:query("select Kills from userMsg,userData where userMsg.loginID=userData.ID and userData.ID='"..loginID.."'")
    mydb.dump(res)
    data['kills']=value

    res=tdb:query("select Deaths from userMsg,userData where userMsg.loginID=userData.ID and userData.ID='"..loginID.."'")
    mydb.dump(res)
    data['deaths']=value
    
    return "@"
end

function mydb.UPDATE(user,level,experience,kills,deaths)
    tdb:query("update userData set Level="..level..",Experience="..experience..",Kills="..kills..",Deaths="..deaths.." where userData.ID= '"..user.."'")
    return "@"
end

function mydb.QUERY(_msg)
    local res={}
    res=tdb:query(_msg)
    return mydb.dump(res)  
end

function mydb.INSERT(user,password,name)
    local time=os.date("%Y-%m-%d")
    tdb:query("insert into userMsg values ('"..user.."','"..password.."','"..name.."','"..time.."')")
    tdb:query("insert into userData values ('"..user.."',"..tonumber('1')..","..tonumber('0')..","..tonumber('0')..","..tonumber('0')..")")
end

skynet.start(function ()
    skynet.error("-------------Mysql Start-------------")

    skynet.register(".mySql")

    mydb.databaseConnect()

    skynet.dispatch("lua",function (_session,_address,_cmd,...)  
        _cmd=_cmd:upper()
        local func=mydb[_cmd]
        if func then
            local ret=nil
            ret=func(...)
            if ret=="@" then
                if _cmd=="GETDATA" then
                    skynet.retpack(data)
                else
                    skynet.retpack("@",value)
                end
                
            else
                skynet.retpack("#")
            end
            
        else
            skynet.error("Cmd Error")
        end
        
    end)
    
end)
