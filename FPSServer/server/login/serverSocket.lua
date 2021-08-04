local skynet=require "skynet"
local socket=require "skynet.socket"

skynet.start(function ()
    print("ServerSocket Start")
    local gateserver=skynet.uniqueservice("gateserver")
    skynet.call(gateserver,"lua","open",{
        port=1234,
        maxclient=64,
        nodelay=true,
    })
end)
