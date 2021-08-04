local redis=require "skynet.db.redis"
local skynet=require "skynet"
local manager=require "skynet.manager"

local db
local myRedis={}
local mySqlMsg

function myRedis.connect()
    db=redis.connect({
        db=0,
        host="127.0.0.1",
        port=6379,
    })
    if not db then
        skynet.error("Redis Error")
    else
        skynet.error("Redis Success")
    end
end

function myRedis.checkOnline(userID)
    local ret=db:hget(userID,"ID")
    if ret then
        skynet.retpack("@")
    else
        skynet.retpack("#")
    end
end

function myRedis.insertClient(userID,name,addr,data)
    db:hmset(userID,
            "ID",userID,
            "name",name,
            "Addr",addr,
            "Level",data['level'],
            "Experience",data['experience'],
            "Kills",data['kills'],
            "Deaths",data['deaths'])
end

function myRedis.updateClientData(accountnumber,level,experience,kills,deaths)
    db:hmset(accountnumber,"Level",level,"Experience",experience,"Kills",kills,"Deaths",deaths)
end

function myRedis.deleteClient(userID,level,experience,kills,deaths)
    skynet.send(".mySql","lua","update",userID,level,experience,kills,deaths)
    db:del(userID)
end

skynet.start(function ()
    
    skynet.error("--------------Redis Start--------------")

    myRedis.connect()

    skynet.register(".redis")

    mySqlMsg=skynet.localname(".mySql")

    skynet.dispatch("lua",function (_session,_address,_cmd,...)
        local func=myRedis[_cmd]
        if func then
            func(...)
        else
            skynet.error("CMD error")
        end
    end)

    
end)