function toggle(i) {
    var tablename = "flights_table" + i;
    var arrowname = "arrow" + i;
    var x = document.getElementById(tablename);
    var arrow = document.getElementById(arrowname);
    if (x.style.display === "none") {
        x.style.display = "block";
        arrow.style["border-bottom"] = "16px solid black";
        arrow.style["border-top"] = "0px";

    } else {
        x.style.display = "none";
        arrow.style["border-bottom"] = "0px";
        arrow.style["border-top"] = "16px solid black";
    }
}