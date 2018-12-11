print("Importing")
import mysql.connector 
import sys
import datetime
from do_post import do_post
print("Done")
if len(sys.argv) < 2:
	print("expecting at least two args") 
	sys.exit(1)
UFID = sys.argv[1]
print(UFID)
mydb = mysql.connector.connect(
    host="localhost",
    user="ethan",
    database="DoorAccess",
    password="genericpassword"
)
mycursor = mydb.cursor()

def Log(UserId):
	InsertSql = "INSERT INTO Log(UserId, date) VALUES (\"{}\", \"{}\")".format(UserId, datetime.datetime.now())
	print(InsertSql)
	mycursor.execute(InsertSql)
	mydb.commit()

def Auth(tier, id):
	LastAccessSql = "SELECT * FROM Log WHERE UserId = %s ORDER BY date DESC"
	mycursor.execute(LastAccessSql, (id, ))
	if tier == 0:
                return True
	elif tier == 1:
		LastAccess = mycursor.fetchone()
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
	mycursor.fetchall()

GetUFIDSql = "SELECT * FROM Users WHERE id = %s"
mycursor.execute(GetUFIDSql, (UFID, ))

myresult = mycursor.fetchall()
myresult = myresult[0]
if Auth(myresult[5], myresult[0]):
#		print(do_post("asdjfasjdf", "ajsdhfj"))
	print("done")
	Log(myresult[0])
# print(do_post(ip, hash))
