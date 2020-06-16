function toggle(i) {
    var tablename = "flights_table" + i;
    console.log(tablename);
    var x = document.getElementById(tablename);
    if (x.style.display === "none") {
        x.style.display = "block";
    } else {
        x.style.display = "none";
    }
}