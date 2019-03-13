<?php 
    header('Content-Type: text/event-stream');
    header('Cache-Control: no-cache');

    $time = "False";
    echo "data: {$time} \n\n";
    flush();
    ?>