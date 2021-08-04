local skynet=require "skynet"
local socket=require "skynet.socket"
require "skynet.manager"

local CMD={}
local table={}
local tablenum=0
local nowclientnum=0
local clientconnect={}
local clientCID={}
local clientperson={}
local redis
local clientName={}

function CMD.OPEN(num)
    if tablenum>0 then
        return
    end
    for i = 1, num do
        local ps=skynet.newservice("person")
        skynet.send(ps,"lua","init",skynet.self())
        table[i]=ps
    end
    tablenum=#table
    skynet.error("Create Person Num:"..tablenum)
end

function CMD.INSERT(cID,addr,user,value)
    if nowclientnum >= tablenum then
        skynet.error("Client Full")
        return "#"
    end
    local num=1
    while true do
        local flag,personaddr=skynet.call(table[num%10],"lua","online",cID,addr,value,user)
        if flag == "@" then
            skynet.error("Client(ID:"..cID.."):Join Success")
            clientperson[cID]=personaddr
            break
        else
            skynet.error("Table(ID:"..table[num%10].."):Full")
        end
        if num<tablenum then
            num=num+1
        else
            skynet.error("Client all Full")
            break;
        end
    end
    nowclientnum=nowclientnum+1
    clientconnect[cID]=addr
    clientCID[cID]=cID
    clientName[cID]=user
    skynet.error("Client:"..nowclientnum)
end

function CMD.DELETE(fd)
    if clientperson[fd] then
        skynet.send(clientperson[fd],"lua","disconnect",fd)
        --skynet.send(".redis","lua","deleteClient",clientName[fd])
        clientconnect[fd]=nil
        clientCID[fd]=nil
        nowclientnum=nowclientnum-1
    end
    skynet.error("Client:"..nowclientnum)
end

function CMD.CLIENTDATA(fd,data)
    if clientperson[fd] then
        skynet.send(clientperson[fd],"lua","gamedata",data)
    end
end

skynet.start(function ()
    skynet.error("----------Agent Start----------")
    skynet.register(".personAgent")
    redis=skynet.localname(".redis")
    CMD.OPEN(10)
    skynet.dispatch("lua",function (session,address,command,...)
        command=command:upper()
        local func=assert(CMD[command])
        if func then
            func(...)
        else
            skynet.error("PersonAgent Command error")
        end
    end)
end)