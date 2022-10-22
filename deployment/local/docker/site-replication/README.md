# Existing
cd /Users/devtools/integration/messaging/rabbit/rabbitmq-docker/site-replication


Create Docker network

```shell
docker network create edge
```

Start Bunny

```shell
./bunny.sh
```

Start Hare

```shell
./hare.sh
```

Access Hare

```shell
docker exec -it hare bash
```





```shell
rabbitmqctl set_parameter federation-upstream origin '{"uri":"amqp://host.docker.internal:5672"}'

rabbitmqctl set_parameter shovel dr-shovel '{"src-protocol": "amqp091", "src-uri": "amqp://host.docker.internal:5672", "src-queue": "dr-products", "dest-protocol": "amqp091", "dest-uri": "amqp://hare", "dest-queue": "backup-product"}'
```

```shell
rabbitmqctl set_policy exchange-federation \
"^pos\." \
'{"federation-upstream-set":"all"}' \
--priority 10 \
--apply-to exchanges
```

Create pos.product 

Publish Message in Local will be sent to hare

-------------

# DC Replication

k port-forward service/rabbitmq 6672:5672

k port-forward service/rabbitmq 16672:15672

open http://localhost:16672

```shell
docker exec -it bunny bash
```

open http://172.16.100.70:15672

```shell
rabbitmqctl -n rabbit set_parameter federation-upstream origin '{"uri":"amqp://retail:retail@172.16.100.70"}'
```
```shell
rabbitmqctl  -n rabbit set_policy exchange-federation \
"^pos\." \
'{"federation-upstream-set":"all"}' \
--priority 10 \
--apply-to exchanges
```

Open 

http://172.16.100.73


```shell
curl -X 'POST' \
  'http://172.16.100.73/amqp/{exchange}/{routingKey}?exchange=pos.products&routingKey=#' \
  -H 'accept: */*' \
  -H 'rabbitContentType: application/json' \
  -H 'Content-Type: application/json' \
  -d '{"id": "11"}'
  ```