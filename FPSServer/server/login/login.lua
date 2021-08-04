local skynet=require "skynet"
local socket=require "skynet.socket"
require "skynet.manager"

local server={}
local redis
local personagent
local mySqlMsg
local user
local password
local name
local value
local loddyAddress="192.168.0.106"
local port="5055"
local data={}

function server.userMsgDe(userMsg)
    local startnum=string.find(userMsg,"@")
    local findnum=string.find(userMsg,":")
    local endnum=string.find(userMsg,"#")
    local user=string.sub(userMsg,startnum+1,findnum-1)
    local password=string.sub(userMsg,findnum+1,endnum-1)
    local name=string.sub(userMsg,endnum+1);
    return user,password,name
end

function server.clientLogin(cID,msg,addr)
    if msg then
            local ret=nil
            user,password=server.userMsgDe(msg)
            local redisRet=skynet.call(redis,"lua","checkOnline",user)
            if redisRet=="@" then
                socket.write(cID,"This account is online#")
                return "#"
            end

            ret,value=skynet.call(mySqlMsg,"lua","query","select name from userMsg where loginID='"..user.."' and password='"..password.."'")
            if ret=="@" then
                data=skynet.call(mySqlMsg,"lua","getdata",user)
                socket.write(cID,"Success@"..value.."&"..loddyAddress..":"..port.."$"..data['level'].."/"..data['experience'].."/"..data['kills'].."/"..data['deaths'].."/")
                skynet.send(redis,"lua","insertClient",user,value,addr,data)
                return "@"
            else
                socket.write(cID,"User or password error#")
            end
        return "#"
    end
end

function server.clientRegister(cID,msg,addr)
    if msg then
        local ret=nil
        user,password,name=server.userMsgDe(msg)
        ret,value=skynet.call(mySqlMsg,"lua","query","select name from userMsg where loginID='"..user.."'")
        if ret=="@" then
            socket.write(cID,"This account already exists#")
        else
            skynet.call(mySqlMsg,"lua","insert",user,password,name)
            socket.write(cID,"RegisterSuccess@"..name)
        end
    end
end


skynet.start(function ()
    skynet.register(".login")
    mySqlMsg=skynet.localname(".mySql")
    personagent=skynet.localname(".personAgent")
    redis=skynet.localname(".redis")
    skynet.dispatch("lua",function (session,address,cmd,fd,addr,msg,...)
        local func=assert(server[cmd])
        if func then
            local ret=func(fd,msg,addr)
            if ret=="@" then
                skynet.send(personagent,"lua","insert",fd,addr,user,value)
            end
        end
    end)
end)
