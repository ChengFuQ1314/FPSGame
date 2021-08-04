local gateserver=require "snax.gateserver"
local skynet = require "skynet"
local netpack=require "skynet.netpack"

local handler={}
local logins_fd={}
local logins_addr={}

local function userCmd(userMsg)
    local startnum=string.find(userMsg,"@")
    local cmd=string.sub(userMsg,1,startnum-1)
    local data=string.sub(userMsg,startnum)
    return cmd,data
end

function handler.connect(fd,addr)
    skynet.error("(Addr:"..addr.."-Fd:"..fd.."):Connect");
    gateserver.openclient(fd)

    local login=skynet.newservice("login",fd,addr)
    logins_fd[fd]=login
    logins_addr[fd]=addr
end

function handler.disconnect(fd)
    skynet.error("Client(Fd:"..fd.."):Disconnect")
    if logins_fd[fd] then
        skynet.send(".personAgent","lua","delete",fd)
        logins_fd[fd]=nil;
        logins_addr[fd]=nil;
    end
end

function handler.error(fd,msg)
    skynet.error("Client(Fd:"..fd.."):Error")
    if logins_fd[fd] then
        skynet.send(".personAgent","lua","delete",fd)
        logins_fd[fd]=nil;
        logins_addr[fd]=nil;
    end
    gateserver.closeclient(fd)
end

function handler.message(fd,msg,sz)
    local _data=netpack.tostring(msg,sz)
    skynet.error("Client(Fd:"..fd.."):".._data)
    local cmd,data=userCmd(_data)

    if logins_fd[fd] then
        if cmd=="login" then
            skynet.send(logins_fd[fd],"lua","clientLogin",fd,logins_addr[fd],data)
        elseif cmd=="Register" then
            skynet.send(logins_fd[fd],"lua","clientRegister",fd,logins_addr[fd],data)
        elseif cmd=="GameData" then
            skynet.send(".personAgent","lua","CLIENTDATA",fd,data);
        end
    end

    
end

gateserver.start(handler)