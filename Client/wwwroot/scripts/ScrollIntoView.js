var Reflectionnaire = Reflectionnaire || {};
Reflectionnaire.scrollToAnswer = function (elementId) {
    if (elementId) {
        const element = document.getElementById(elementId);
        element.scrollIntoView({ block: "center" });
    }
};