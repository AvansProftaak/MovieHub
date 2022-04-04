function countCheckbox()
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

function validateHhMm(inputField) 
{
    var isValid = /^([0-1]?[0-9]|2[0-4]):([0-5][0-9])(:[0-5][0-9])?$/.test(inputField.value);

    if (isValid) {
        inputField.style.backgroundColor = '#bfa';
        document.getElementById('addButton').disabled = false;

    } else {
        inputField.style.backgroundColor = '#fba';
        document.getElementById('addButton').disabled = true;
    }
    return isValid;
}