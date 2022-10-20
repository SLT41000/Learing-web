function bindClass() {
    removeElementsByClass("col-xs-12 col-sm-4")
    m = $('#classDrpDwn').val();
    createcard(m);
    
    

   
   
}

function removeElementsByClass(className) {
    const elements = document.getElementsByClassName(className);
    while (elements.length > 0) {
        elements[0].parentNode.removeChild(elements[0]);
    }
}

function createcard(m) {
    
    const main = document.querySelector('.main-card');
    const card = document.createElement('div');
    card.className = "row";
    
    GDATA.forEach((e, i) => {
        if (m != e.mid) {
            return
        }
        
        if (e.mid == "1") {
            img = "img/phy.png";
        } else if (e.mid=="2") {
            img = "img/Chemical.png";
        } else if (e.mid == "3") {
            img = "img/Biology.jfif";
        } else if (e.mid == "4") {
            img = "img/Calculus.png";
        } else if (e.mid == "5") {
            img = "img/Stt.png";
        } else if (e.mid == "6") {
            img = "img/Applied mathematics.jfif";
        } 
       

        const cardcontent = `
        
    <div class="col-xs-12 col-sm-4">
                        <div class="card" id="mid-${e.mid}">
                            <a class="img-card" href="watch.aspx?vid=${e.vid}&vname=${e.vname}&vlink=${e.vlink}&vdes=${e.description}">
                            <img src="${img}" />
                          </a>
                            <div class="card-content">
                                <h4 class="card-title">
                                    <a href="watch.aspx?vid=${e.vid}&vname=${e.vname}&vlink=${e.vlink}&vdes=${e.description}"> ${e.vname}
                                  </a>
                                </h4>

                            </div>
                            <div class="card-read-more">
                                
                            </div>
                        </div>
                           
                    </div>
`
        card.innerHTML += cardcontent;
        main.appendChild(card);
})
   
    
}
var GDATA;
$(document).ready(function () {
    let m = $('#classDrpDwn').val();

    console.log(l);
    $.ajax({
        type: 'GET',
        url: '/service1.svc/getscreendata',
        dataType: 'json',
        success: (data) => {
            GDATA = data;

            
            createcard(m);
            
        },

    });
});
