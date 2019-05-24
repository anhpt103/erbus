// models / data
var matHangApp = angular.module("matHangApp", []);
matHangApp.controller('matHangAppCtrl', ['$scope', '$http',
       function ($scope, $http) {
           var hostname = window.location.hostname;
           var port = window.location.port;
           var rootUrlApi = 'http://localhost:55296';
           var serviceUrl = rootUrlApi + '/api/Catalog/MatHang/GetAllMatHang/' + 'A000001';

           var items = new kendo.data.DataSource({
               schema: {
                   parse: function (data) {
                       return data.Data;
                   },
                   data: function (data) {
                       return data.Data;
                   },
                   total: function (data) {
                       if (data && data.Data && data.Data.length > 0) return data.Data.length;
                       else return 0;
                   },
                   model: {
                       id: "ID",
                       fields: {
                           MAHANG: { type: "string" },
                       }
                   }
               },
               transport:
               {
                   read: {
                       type: "GET",
                       url: serviceUrl,
                       headers: { 'Authorization': 'Bearer Z-oNFgOQD6GqEK1XDvUHF3kW4rQjefB54388rcLAcC_a5wuz8DvzNPZI7s4w0r92GWutdTye95efPsvRSurWplAdU3xnSw-Lim4QKAqyRvGX8HZ7pH_XkUhDJAputr4DhDGMZ91FEe26K4bVUkWyzfD99wz_usBcUHzurfMQzJGPQ8CM_CbQKwW4lP-xjSbIix2qvjfgBhbJWk80SwlCuP2NQqZxr97LdfcN17DlMmiZkBJVMEJ5OYHJ-4DFaUzSLVJzSds1dmgUoSKPEMwBtjRx3tKe_4ZCKEay1ywZ8BNKt1gUlv5iFcRRJ-qdEg03rWFePVo4Yuu3F3MNBBtsIJXpMNqe6ITZT7UCnBzm8B0'},
                       contentType: "application/json; charset=utf-8",
                       dataType: 'json',
                       success: function (result) {
                           console.log(result);
                           options.success(result);
                       }
                   }
               },
               parameterMap: function (options, operation) {
                   if (operation == "read") {
                       return JSON.stringify('A000001');
                   }
               }
           });

           var cart = kendo.observable({
               contents: [],
               cleared: false,

               contentsCount: function () {
                   return this.get("contents").length;
               },

               add: function (item) {
                   var found = false;

                   this.set("cleared", false);

                   for (var i = 0; i < this.contents.length; i++) {
                       var current = this.contents[i];
                       if (current.item === item) {
                           current.set("quantity", current.get("quantity") + 1);
                           found = true;
                           break;
                       }
                   }

                   if (!found) {
                       this.contents.push({ item: item, quantity: 1 });
                   }
               },

               remove: function (item) {
                   for (var i = 0; i < this.contents.length; i++) {
                       var current = this.contents[i];
                       if (current === item) {
                           this.contents.splice(i, 1);
                           break;
                       }
                   }
               },

               empty: function () {
                   var contents = this.get("contents");
                   contents.splice(0, contents.length);
               },

               clear: function () {
                   var contents = this.get("contents");
                   contents.splice(0, contents.length);
                   this.set("cleared", true);
               },

               total: function () {
                   var price = 0,
                       contents = this.get("contents"),
                       length = contents.length,
                       i = 0;

                   for (; i < length; i++) {
                       price += parseInt(contents[i].item.price) * contents[i].quantity;
                   }

                   return kendo.format("{0:c}", price);
               }
           });

           var layoutModel = kendo.observable({
               cart: cart
           });

           var cartPreviewModel = kendo.observable({
               cart: cart,

               cartContentsClass: function () {
                   return this.cart.contentsCount() > 0 ? "active" : "empty";
               },

               removeFromCart: function (e) {
                   this.get("cart").remove(e.data);
               },

               emptyCart: function () {
                   cart.empty();
               },

               itemPrice: function (cartItem) {
                   return kendo.format("{0:c}", cartItem.item.price);
               },

               totalPrice: function () {
                   return this.get("cart").total();
               },

               proceed: function (e) {
                   this.get("cart").clear();
                   matHang.navigate("/");
               }
           });

           var indexModel = kendo.observable({
               items: items,
               cart: cart,

               addToCart: function (e) {
                   cart.add(e.data);
               }
           });
           var detailModel = kendo.observable({
               imgUrl: function () {
                   return "/images/200/" + this.get("current").image
               },

               price: function () {
                   return kendo.format("{0:c}", this.get("current").price);
               },

               addToCart: function (e) {
                   cart.add(this.get("current"));
               },

               setCurrent: function (itemID) {
                   this.set("current", items.get(itemID));
               },

               previousHref: function () {
                   var id = this.get("current").id - 1;
                   if (id === 0) {
                       id = items.data().length - 1;
                   }

                   return "#/MAHANG/" + id;
               },

               nextHref: function () {
                   var id = this.get("current").id + 1;

                   if (id === items.data().length) {
                       id = 1;
                   }

                   return "#/MAHANG/" + id;
               },

               kCal: function () {
                   return kendo.toString(this.get("current").stats.energy / 4.184, "0.0000");
               }
           });

           // Views and layouts
           var layout = new kendo.Layout("layout-template", { model: layoutModel });
           var cartPreview = new kendo.Layout("cart-preview-template", { model: cartPreviewModel });
           var index = new kendo.View("index-template", { model: indexModel });
           var checkout = new kendo.View("checkout-template", { model: cartPreviewModel });
           var detail = new kendo.View("detail-template", { model: detailModel });

           var matHang = new kendo.Router({
               init: function () {
                   console.log("router init")
                   layout.render("#application");
               }
           });

           var viewingDetail = false;

           // Routing
           matHang.route("/", function () {
               console.log("router root route")
               viewingDetail = false;
               layout.showIn("#content", index);
               layout.showIn("#pre-content", cartPreview);
           });

           matHang.route("/checkout", function () {
               viewingDetail = false;
               layout.showIn("#content", checkout);
               cartPreview.hide();
           });

           matHang.route("/menu/:id", function (itemID) {
               layout.showIn("#pre-content", cartPreview);
               var transition = "",
                   current = detailModel.get("current");

               if (viewingDetail && current) {
                   transition = current.id < itemID ? "tileleft" : "tileright";
               }
               console.log(items);
               items.fetch(function (e) {
                   if (detailModel.get("current")) { // existing view, start transition, then update content. This is necessary for the correct view transition clone to be created.
                       layout.showIn("#content", detail, transition);
                       detailModel.setCurrent(itemID);
                   } else {
                       // update content first
                       detailModel.setCurrent(itemID);
                       layout.showIn("#content", detail, transition);
                   }
               });

               viewingDetail = true;
           });

           $(function () {
               matHang.start();
           });

       }]);
