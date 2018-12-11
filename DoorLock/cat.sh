#!/bin/bash
while :	
do
	UID_LINE=$(nfc-poll | grep -oE "UID \(NFC.*")
	UID_SPACES=$(echo $UID_LINE | sed -r "s_.*: ([0-9]+.*)_\1_g")
	UFID=$(echo $UID_SPACES | sed "s_ __g")
	python3 main.py $UFID
done
