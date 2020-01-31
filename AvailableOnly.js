var filters = document.getElementsByClassName('bui-checkbox');
for (var i = 0; i < filters.length; i++) {
    if (filters[i].innerText.includes('доступные варианты')) {
        filters[i].getElementsByClassName('bui-checkbox__input js-bui-checkbox__input')[0].click();
        break;
    }
}