let params = new URLSearchParams(location.search);
let v = params.get('vlink');
let h=params.get('vname');
let p = params.get('vdes');



$("#video").attr('src', v);
document.getElementById("headname").textContent = h;
document.getElementById("p").textContent = p;

