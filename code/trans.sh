#! /bin/bash

cat test.txt | nc 192.168.2.2 8888
cat /dev/null > test.txt

#rm test.txt
