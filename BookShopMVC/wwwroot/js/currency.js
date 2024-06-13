function setCurrency(currencyCode) {
    console.log("Setting currency:", currencyCode);
    document.cookie = "CurrencyCode=" + currencyCode + ";path=/";
    console.log(currencyCode);
    location.reload();
}