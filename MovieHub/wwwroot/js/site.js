﻿function countCheckbox()
{
    if (document.getElementById("roleName").innerText === "Admin")
    {
        let counter = $('input:checkbox:checked').length;

        if (counter === 0)
        {
            document.getElementById("counter").style.display = "inherit";
            document.getElementById("counter").innerText = "You need to add at least 1 admin!";
            document.getElementById("submitAdmin").disabled = true;
        }

        if (counter > 0)
        {
            document.getElementById("counter").style.display = "none";
            document.getElementById("submitAdmin").disabled = false;
        }
    }
}

function toCreateMovie() 
{
    location.href = '/Movies/Create';
}
