

# High Availability with GemFire for Redis Applications

Test Case

- Start GemFire Cluster across 2 servers
- There will 1 locators - 1 data node on each servers
- The total of 2 locators and 2 data nodes across two servers
- Push records to 1 GemFire
- Stop data node on Server 1
- Verify no data loass
- Restart data node on Server 1
- Stop one data node on Server 2
- Verify no data loass
- Stop Locator on Server 2 (total lost of one physical server)
- Verify no data loass

# GemFire Startup

Local environment

Start Locator Server 1

```shell
start locator --name=locator1 --port=10334 --locators="127.0.0.1[10334],127.0.0.1[10434]" --bind-address=127.0.0.1 --hostname-for-clients=127.0.0.1  --jmx-manager-hostname-for-clients=127.0.0.1 --http-service-bind-address=127.0.0.1
```


Start Locator Server 2



```shell
start locator --name=locator2 --port=10434 --locators="127.0.0.1[10334],127.0.0.1[10434]"  --bind-address=127.0.0.1 --hostname-for-clients=127.0.0.1  --http-service-port=0 --J="-Dgemfire.jmx-manager-port=1098"
```


Start Redis Server 1

```shell
start server --name=redisServer1   --locators="127.0.0.1[10334],127.0.0.1[10434]"  --server-port=40401 --bind-address=127.0.0.1 --hostname-for-clients=127.0.0.1 --start-rest-api=true --http-service-bind-address=127.0.0.1 --http-service-port=0  --J=-Dgemfire-for-redis-port=6379 --J=-Dgemfire-for-redis-enabled=true --classpath=/Users/devtools/repositories/IMDG/gemfire/gemfire-for-redis-apps-1.0.1/lib/*
```


Start Redis Server 2

```shell
start server --name=redisServer2   --locators="127.0.0.1[10334],127.0.0.1[10434]"  --server-port=40402 --bind-address=127.0.0.1 --hostname-for-clients=127.0.0.1 --start-rest-api=true --http-service-bind-address=127.0.0.1 --http-service-port=0  --J=-Dgemfire-for-redis-port=6372 --J=-Dgemfire-for-redis-enabled=true --classpath=/Users/devtools/repositories/IMDG/gemfire/gemfire-for-redis-apps-1.0.1/lib/*
```

# Starting Rest Application

Start App

```shell
cd applications/pos-service/
dotnet run --REDIS_CONNECTION_STRING="localhost:6379,localhost:6372,connectRetry=10"
```

Post Product data 

```shell
curl -d '{"id":"sku1", "name":"PeanutButter"}' -H "Content-Type: application/json" -X POST http://localhost:5001/api/product"
```

Get Product data
```shell
curl http://localhost:5001/api/product/sku1
```

# High Availability Testings



- Kill one server
```shell
kill -9 `ps -ef | grep "start redisServer1" | grep -v "grep" | awk '{ print $2 }'`

Get Product data
```shell
curl http://localhost:5001/api/product/sku1
```



Restart server

```shell
start server --name=redisServer1   --locators="127.0.0.1[10334],127.0.0.1[10434]"  --server-port=40401 --bind-address=127.0.0.1 --hostname-for-clients=127.0.0.1 --start-rest-api=true --http-service-bind-address=127.0.0.1 --http-service-port=0  --J=-Dgemfire-for-redis-port=6379 --J=-Dgemfire-for-redis-enabled=true --classpath=/Users/devtools/repositories/IMDG/gemfire/gemfire-for-redis-apps-1.0.1/lib/*
```

- Kill two server
```shell
kill -9 `ps -ef | grep "start redisServer2" | grep -v "grep" | awk '{ print $2 }'`

Get Product data
```shell
curl http://localhost:5001/api/product/sku1
```


- Kill two locator (full server outage)

```shell
kill -9 `ps -ef | grep "start locator2" | grep -v "grep" | awk '{ print $2 }'`

Get Product data
```shell
curl http://localhost:5001/api/product/sku1
```
