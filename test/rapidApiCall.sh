#!/bin/bash
for i in {1..10}
do
  curl --location --request POST 'localhost:5000/' \
  --header 'Content-Type: application/json' \
  --insecure \
  --data-raw '{
      "message": {
          "attributes": {
              "key": "value"
          },
          "data": "SGVsbG8gQ2xvdWQgUHViL1N1YiEgSGVyZSBpcyBteSBtZXNzYWdlIQ==",
          "messageId": "136969346945"
      },
    "subscription": "projects/myproject/subscriptions/mysubscription"
  }'
done