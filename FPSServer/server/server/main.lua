local skynet = require "skynet"

skynet.start(function ()

    print("---------Start---------")
    

    skynet.uniqueservice(true,"mySql")

    skynet.uniqueservice(true,"redis")

    skynet.uniqueservice(true,"serverSocket")

    skynet.uniqueservice(true,"personAgent")

    print("---------End---------")

end)
