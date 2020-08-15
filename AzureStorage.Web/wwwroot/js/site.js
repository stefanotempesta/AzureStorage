window.JavaScriptFunctions = {
    toggleVisibility: function (id) {
        console.log(id);
        var visibility = document.getElementById(id).style.visibility;
        document.getElementById(id).style.visibility = visibility === "visible" ? "hidden" : "visible";
    },

    getValue: function (id) {
        console.log(id);
        var value = document.getElementById(id).value;
        return value;
    },

    alertDanger: function (strong, text) {
        JavaScriptFunctions.toggleVisibility('alertDanger');
        document.getElementById('alertDangerStrong').innerText = strong;
        document.getElementById('alertDangerText').innerText = text;
    }
};

window.QueueFunctions = {
    toggleMessageText: function (id) {
        console.log(id);
        JavaScriptFunctions.toggleVisibility('textQueueMessage_' + id);
        JavaScriptFunctions.toggleVisibility('formEditQueueMessage_' + id);
    }
};