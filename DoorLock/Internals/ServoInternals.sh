echo "INSTALLING"
sudo apt-get install apache2 -y
sudo apt-get install php -y
sudo apt-get install python3 -y
echo "INIT"
sudo rm -r /var/www/html
sudo mkdir /var/www/html
cat "TBD" > Open.php
cat "TBD" > ServoOpen.py
cat "TBD" > ServoClose.py
echo "PERMISSION ASSIGNMENTS"
sudo chown -R www-data:www-data /var/www/html
sudo chown -R 764 /var/www/html

sudo systemctl reload apache2
echo "DONE"
