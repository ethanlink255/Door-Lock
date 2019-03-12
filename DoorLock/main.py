print("Importing")
import mysql.connector 
import sys
import datetime
import subprocess
from do_post import do_post
print("Done")
mydb = mysql.connector.connect(
    host="localhost",
    user="ethan",
    database="DoorAccess",
    password="genericpassword"
)
mycursor = mydb.cursor(buffered=True)

def Log(UserId):
	InsertSql = "INSERT INTO Log(UserId, date) VALUES (\"{}\", \"{}\")".format(UserId, datetime.datetime.now())
	print(InsertSql)
	global mycursor
	mycursor.execute(InsertSql)
	mydb.commit()

def Auth(tier, id):
	LastAccessSql = "SELECT * FROM Log WHERE UserId = %s ORDER BY date DESC LIMIT 1"
	global mycursor
	mycursor.execute(LastAccessSql, (id, ))
	if tier == 0:
                return True
	elif tier == 1:
		LastAccess = mycursor.fetchone()
#		mycursor.fetchall()
		if not LastAccess:
			return True
		if LastAccess[2].date() != datetime.datetime.today().date():
			return True
		else:
			return False
	elif tier == 2:
		if 9 <= datetime.datetime.now() < 18:
			return True
	else:
		return False


def ProcessUID(UFID):
	GetUFIDSql = "SELECT * FROM Users WHERE id = %s"
	global mycursor
	mycursor.execute(GetUFIDSql, (UFID, ))

	myresult = mycursor.fetchall()
	myresult = myresult[0]
	if Auth(myresult[5], myresult[0]):
	#		print(do_post("asdjfasjdf", "ajsdhfj"))
		print("done")
		Log(myresult[0])
	# print(do_post(ip, hash))


while True:
	try:
		Lines = subprocess.check_output('nfc-poll').decode().split('\n')
		for line in Lines:
			if "UID (NFC" in line:
				ID = line.split(':')[1]
				UFID = ID.replace(" ", "")
				ProcessUID(UFID)
				break
	except:
		pass


