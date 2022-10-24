

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

Generate password
```shell
java -DCRYPTION_KEY=PIVOTAL -classpath /Users/Projects/VMware/Tanzu/TanzuData/TanzuGemFire/dev/gemfire-extensions/applications/libs/nyla.solutions.core-1.5.1.jar nyla.solutions.core.util.Cryption $1
```

```shell
export CLASSPATH="/Users/devtools/repositories/IMDG/gemfire/vmware-gemfire-9.15.0/lib/HikariCP-4.0.3.jar:/Users/devtools/repositories/RDMS/PostgreSQL/driver/postgresql-42.2.9.jar:/Users/Projects/VMware/Tanzu/TanzuData/TanzuGemFire/dev/gemfire-extensions/applications/libs/nyla.solutions.core-1.5.1.jar:/Users/Projects/VMware/Tanzu/TanzuData/TanzuGemFire/dev/gemfire-extensions/components/gemfire-extensions-core/build/libs/gemfire-extensions-core-1.0.0.jar:/Users/devtools/repositories/IMDG/gemfire/gemfire-for-redis-apps-1.0.1/lib/*"
export JDBC_URL=jdbc:postgresql://localhost:5432/postgres
export JDBC_DRIVER_CLASS=org.postgresql.Driver
export JDBC_USERNAME=postgres
export JDBC_PASSWORD=CRYPTED_PASSWORD_HERE
```

```shell
cd /Users/devtools/repositories/IMDG/gemfire/vmware-gemfire-9.15.0/bin
./gfsh
```
```shell
start locator --name=gf-locator1 --port=10334 --locators="127.0.0.1[10334],127.0.0.1[10434]" --bind-address=127.0.0.1 --hostname-for-clients=127.0.0.1  --jmx-manager-hostname-for-clients=127.0.0.1 --http-service-bind-address=127.0.0.1
```
```shell
start locator --name=gf-locator2 --port=10434 --locators="127.0.0.1[10334],127.0.0.1[10434]"  --bind-address=127.0.0.1 --hostname-for-clients=127.0.0.1  --http-service-port=0 --J="-Dgemfire.jmx-manager-port=1098"
```

```shell
configure pdx --disk-store --read-serialized=true
```

```shell
start server --name=gf-server1 --initial-heap=500m --max-heap=500m  --locators="127.0.0.1[10334],127.0.0.1[10434]"  --server-port=40401 --bind-address=127.0.0.1 --hostname-for-clients=127.0.0.1 --start-rest-api=true --http-service-bind-address=127.0.0.1 --http-service-port=9090  --J=-Dgemfire-for-redis-port=6379 --J=-Dgemfire-for-redis-enabled=true --J=-Dgemfire-for-redis-region-name=REDIS_DATA --J=-Dgemfire-for-redis-use-default-region-config=false --include-system-classpath=true --J=-DCRYPTION_KEY=PIVOTAL --J=-Dconfig.properties=/Users/Projects/VMware/Tanzu/TanzuData/TanzuGemFire/dev/gemfire-extensions/deployments/gemfire-server/config/gf-extensions.properties
```

```shell
start server --name=gf-server2 --initial-heap=500m --max-heap=500m  --locators="127.0.0.1[10334],127.0.0.1[10434]"  --server-port=40402 --bind-address=127.0.0.1 --hostname-for-clients=127.0.0.1 --start-rest-api=true --http-service-bind-address=127.0.0.1 --http-service-port=9092  --J=-Dgemfire-for-redis-port=6372 --J=-Dgemfire-for-redis-enabled=true --J=-Dgemfire-for-redis-region-name=REDIS_DATA --J=-Dgemfire-for-redis-use-default-region-config=false  --include-system-classpath=true --J=-DCRYPTION_KEY=PIVOTAL --J=-Dconfig.properties=/Users/Projects/VMware/Tanzu/TanzuData/TanzuGemFire/dev/gemfire-extensions/deployments/gemfire-server/config/gf-extensions.properties

```


Create custom region

```shell
create region --name=REDIS_DATA --type=PARTITION_REDUNDANT_PERSISTENT_OVERFLOW  --redundant-copies=1
```

```shell
create region --name=customers  --type=PARTITION --cache-loader=com.vmware.data.services.gemfire.integration.jdbc.JdbcJsonPdxLoader --cache-writer=com.vmware.data.services.gemfire.integration.jdbc.JdbcJsonCacheWriter
```

# Starting Service API

Start App

```shell
cd /Users/Projects/VMware/Tanzu/Use-Cases/Vertical-Industries/VMware-Retail/dev/vmware-pos-data-showcas/applications/pos-service/

export REDIS_CONNECTION_STRING="localhost:6379,localhost:6372,connectRetry=10"
dotnet run
```

# Start Consumer


```shell
cd /Users/Projects/VMware/Tanzu/Use-Cases/Vertical-Industries/VMware-Retail/dev/vmware-pos-data-showcas
cd applications/pos-consumer
export REDIS_CONNECTION_STRING="localhost:6379,localhost:6372,connectRetry=10"
export RABBIT_HOST="localhost"
export RABBIT_USERNAME="guest"
export RABBIT_PASSWORD="guest"
export RABBIT_CLIENT_NAME="pos-consumer"
export CRYPTION_KEY="SECRET"

dotnet run
```


Open Source Publisher in DC

```shell
open http://172.16.100.73
```

```properties
exchange=pos.products
```

```json
{ 
    "id" : "1", 
    "name" : "Peanut Butter", 
    "price" : 2.76, 
    "details" : "Complete satisfaction or your money back. Scan for more food information.", 
    "ingredients" : "Roasted Peanuts, Contains 2% Or Less Of: Molasses, Fully Hydrogenated Vegetable Oils (Rapeseed And Soybean), Mono And Diglycerides, Salt.", 
    "directions" : "Ready to eat.", 
    "warnings" : " Allergens: Contains Peanuts.", 
    "quantityAmount" : "28oz", 
    "nutrition" :  {
        "totalFatAmount" : 16,
        "cholesterol" :0,
        "sodium" : 140,
        "totalCarbohydrate" : 8,
        "sugars" : 3,
        "protein" : 7
    }}
```


```json
{ 
    "id" : "2", 
    "name" : "Jelly", 
    "price" : 3.16, 
    "details" : "50 calories per 1 tbsp.", 
    "ingredients" : "fruit juice, sugar, and pectin.", 
    "directions" : "Refrigerate after opening.", 
    "warnings" : "Button Will Pop Up When Seal Is Broken.", 
    "quantityAmount" : "18oz", 
    "nutrition" :  {
        "totalFatAmount" : 16,
        "cholesterol" :0,
        "sodium" : 140,
        "totalCarbohydrate" : 8,
        "sugars" : 3,
        "protein" : 7
    }}
```

Get Product data
```shell
curl http://localhost:5001/api/product/1
```
```shell
curl http://localhost:5001/api/product/2
```

# High Availability Testings


Open Pulse

```shell
open http://localhost:7070/pulse/login.html
``


- Kill one server
```shell
kill -9 `ps -ef | grep "start gf-server1" | grep -v "grep" | awk '{ print $2 }'`

Get Product data
```shell
curl http://localhost:5001/api/product/1
```

```shell
curl http://localhost:5001/api/product/2
```



Restart server

```shell
start server --name=gf-server1 --initial-heap=500m --max-heap=500m  --locators="127.0.0.1[10334],127.0.0.1[10434]"  --server-port=40401 --bind-address=127.0.0.1 --hostname-for-clients=127.0.0.1 --start-rest-api=true --http-service-bind-address=127.0.0.1 --http-service-port=9090  --J=-Dgemfire-for-redis-port=6379 --J=-Dgemfire-for-redis-enabled=true --J=-Dgemfire-for-redis-region-name=REDIS_DATA --J=-Dgemfire-for-redis-use-default-region-config=false --include-system-classpath=true --J=-DCRYPTION_KEY=PIVOTAL --J=-Dconfig.properties=/Users/Projects/VMware/Tanzu/TanzuData/TanzuGemFire/dev/gemfire-extensions/deployments/gemfire-server/config/gf-extensions.properties
```

- Kill two server
```shell
kill -9 `ps -ef | grep "start gf-server2" | grep -v "grep" | awk '{ print $2 }'`

Get Product data
```shell
curl http://localhost:5001/api/product/1
```


- Kill two locator (full server outage)

```shell
kill -9 `ps -ef | grep "start gf-locator2" | grep -v "grep" | awk '{ print $2 }'`

Get Product data
```shell
curl http://localhost:5001/api/product/1
```


Shutdown Servers

```shell
cd /Users/devtools/repositories/IMDG/gemfire/vmware-gemfire-9.15.0/bin
./gfsh
```


```
connect
shutdown --include-locators
```


Restart cluster

```
 curl http://localhost:5001/api/product/1
 ```
-----------------------------------------------
# Caching Database Synchronization


In Postgres

```shell
psql -d postgres -U postgres
```

```sqlite-sql
drop table customers;

CREATE TABLE customers (
email varchar(255),
"firstName" varchar(255),
"lastName" varchar(255),
PRIMARY KEY (email)
);

    
```



```shell
curl -X 'PUT' \
'http://localhost:9090/geode/v1/customers?keys=jdoe%40vmware.com&op=PUT' \
-H 'accept: application/json;charset=UTF-8' \
-H 'Content-Type: application/json;charset=UTF-8' \
-d '{
"email" : "jdoe@vmware.com",
"firstName" : "Jane",
"lastName" : "Doe"
}'   

curl -X 'PUT' \
'http://localhost:9090/geode/v1/customers?keys=gdoe%40vmware.com&op=PUT' \
-H 'accept: application/json;charset=UTF-8' \
-H 'Content-Type: application/json;charset=UTF-8' \
-d '{
"email" : "gdoe@vmware.com",
"firstName" : "Gill",
"lastName" : "Doe"
}'   

curl -X 'PUT' \
'http://localhost:9090/geode/v1/customers?keys=jsmith%40vmware.com&op=PUT' \
-H 'accept: application/json;charset=UTF-8' \
-H 'Content-Type: application/json;charset=UTF-8' \
-d '{
"email" : "jsmith@vmware.com",
"firstName" : "John",
"lastName" : "Smith"
}'   

curl -X 'PUT' \
'http://localhost:9090/geode/v1/customers?keys=msmith%40vmware.com&op=PUT' \
-H 'accept: application/json;charset=UTF-8' \
-H 'Content-Type: application/json;charset=UTF-8' \
-d '{
"email" : "msmith@vmware.com",
"firstName" : "Mary",
"lastName" : "Smith"
}'   
```

In gfsh

http://localhost:9090/geode/swagger-ui/index.html


```shell
curl -X 'GET' \
  'http://localhost:9090/geode/v1/customers?limit=50&keys=jdoe@vmware.com' \
  -H 'accept: application/json;charset=UTF-8'
```



query --query="select * from  /customers where lastName like  'S%'"



Got http://localhost:9090/geode/swagger-ui/index.html#/queries/runAdhocQuery
select * from  /customers where lastName like  'S%25'
or run
```shell
curl -X 'GET' \
  'http://localhost:9090/geode/v1/queries/adhoc?q=select%20%2A%20from%20%20%2Fcustomers%20where%20lastName%20like%20%20%27S%2525%27' \
  -H 'accept: application/json;charset=UTF-8'
```


Insert into Database


Pslq 
```
INSERT INTO customers
(email, "firstName", "lastName")
VALUES('tevans@company', 'Therman', 'Evans');

```


For into Cache

```shell
curl -X 'GET' \
  'http://localhost:9090/geode/v1/customers?limit=50&keys=tevans@company' \
  -H 'accept: application/json;charset=UTF-8'
```

----------------------------------

Open DC RabbitMQ Cluster


```
open http://172.16.100.70:15672/#/
```

Open Local (Edge) RabbitMQ active


```
open http://localhost:15672/
```

Passive DR RabbitMQ (local-edge)

```
open http://localhost:15670
```

Open k9s

Kill Rabbit


```shell
k delete pod rabbitmq-server-0
```

- Publish Product
- Verify Passive DR RabbitMQ (local-edge)



## Scale Up RabbitMQ

```shell
k edit RabbitMQCluster rabbitmq
```

Change replicas to 3



```shell
k delete pod rabbitmq-server-0
```


- Publish Product
- Verify Passive DR RabbitMQ (local-edge)


```shell
k delete pod rabbitmq-server-1
```


- Publish Product
- Verify Passive DR RabbitMQ (local-edge)

# SQL HA

