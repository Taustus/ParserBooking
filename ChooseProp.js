var filters = document.getElementsByClassName('bui-checkbox');
for (var i = 0; i < filters.length; i++) {
    if (filters[i].innerText.includes('Отели') && hotels) {
        filters[i].getElementsByClassName('bui-checkbox__input js-bui-checkbox__input')[0].click();
        hotels = false;
        if (!hotels && !avail) {
            break;
        }
    }
    if (filters[i].innerText.includes('Апартаменты') && apart) {
        filters[i].getElementsByClassName('bui-checkbox__input js-bui-checkbox__input')[0].click();
        apart = false;
        if (!hotels && !apart) {
            break;
        }
    }
}