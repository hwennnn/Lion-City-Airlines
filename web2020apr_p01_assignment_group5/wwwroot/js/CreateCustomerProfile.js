function myFunction() {
    var d = new Date();
    document.getElementById("date").innerHTML = d;
    customername = document.getElementById("CustomerName").value;
    alert(`Thank you for Signing Up Lion City Airlines, ${customername}\nYour account has been activated sucessfully!\nYour Current PassWord: "p@55Cust".\nDate: ${d}`);
}