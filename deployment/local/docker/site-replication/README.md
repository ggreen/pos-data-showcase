# Existing
/Users/devtools/integration/messaging/rabbit/rabbitmq-docker/site-replication


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
rabbitmqctl set_parameter federation-upstream origin '{"uri":"amqp://bunny:5672"}'
```
```shell
rabbitmqctl set_policy exchange-federation \
"^pos\." \
'{"federation-upstream-set":"all"}' \
--priority 10 \
--apply-to exchanges
```

Create pos.product 

Publish Message in Hare will be sent to bunny
