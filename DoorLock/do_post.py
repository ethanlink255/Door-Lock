import requests

def do_post(ip_addr,  hash):
    r = requests.post("https://" + ip_addr + "/" + "Open.php",
                      data={'auth': hash})
    return r.status_code, r.reason

