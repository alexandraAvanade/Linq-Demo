using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Demo
{
    public class Esercizio
    {
        //Creazione liste
        public static List<Product> CreateProductList()
        {
            var lista = new List<Product>
            {
                new Product {Id=1, Name = "Telefono", UnitPrice=300.99},
                new Product{Id=2, Name = "Computer", UnitPrice = 800},
                new Product{Id = 3, Name = "Tablet", UnitPrice = 550.99}
            };
            return lista;
        }

        public static List<Order> CreateOrderList(){
            var lista = new List<Order>();

            var order = new Order { 
            Id = 1,
            ProductId = 1,
            Quantity = 4};

            lista.Add(order);

            var order1 = new Order
            {
                Id = 2,
                ProductId = 2,
                Quantity = 1
            };

            lista.Add(order1);

            var order2 = new Order
            {
                Id = 3,
                ProductId = 1,
                Quantity = 1
            };

            lista.Add(order2);


            return lista;
        }

        //Esecuzione immediata e ritardata
        public static void DeferredExecution()
        {
            var productList = CreateProductList();
            var orderList = CreateOrderList();

            //Vediamo i risultati
            foreach( var p in productList)
            {
                Console.WriteLine("{0} - {1} - {2}",p.Id, p.Name, p.UnitPrice);
            }
            Console.WriteLine("");

            foreach ( var o in orderList)
            {
                
                Console.WriteLine("{0} - {1} - {2}", o.Id, o.ProductId, o.Quantity);
            }


            //Creazione Query
            var list = productList
                .Where(product => product.UnitPrice >= 400)
                .Select(p => new { Nome = p.Name, Prezzo = p.UnitPrice});


            //Aggiungo prodotto

            productList.Add(new Product { 
            Id = 4,
            Name = "Bici",
            UnitPrice = 500.99});


            //Risultati
            //Esecuzione Differita

            Console.WriteLine("");
            Console.WriteLine("Esecuzione Differita: ");
             foreach(var p in list)
            {
                
                Console.WriteLine("{0} - {1}", p.Nome, p.Prezzo);
            }

            //Esecuzione immediata

            var list1 = productList
               .Where(p => p.UnitPrice >= 400)
               .Select(p => new { Nome = p.Name, Prezzo = p.UnitPrice })
               .ToList();

            productList.Add(new Product { Id = 5, Name = "Divano", UnitPrice = 450.99 });

            //Risultati
            Console.WriteLine("");
            Console.WriteLine("Esecuzione immediata: ");

            foreach(var p in list1)
            {
                Console.WriteLine("{0} - {1}", p.Nome, p.Prezzo);
            }

            
        }

        //Sintassi
        public static void Syntax()
        {
            var productList = CreateProductList();
            var orderList = CreateOrderList();

            //Method Syntax
            var methodList = productList
               .Where(p => p.UnitPrice <= 600)
               .Select(p => new { Nome = p.Name, Prezzo = p.UnitPrice })
               .ToList();

            //Query Syntax
            var queryList =
                (from p in productList
                where p.UnitPrice <= 600
                select new { Nome = p.Name, Prezzo = p.UnitPrice }).ToList();
            
        }

        //Operatori
        public static void Operators()
        {
            var productList = CreateProductList();
            var orderList = CreateOrderList();

            //Scritura a schermo delle liste

            Console.WriteLine("Lista Prodotti: ");
            foreach (var p in productList)
            {
                Console.WriteLine("{0} - {1} - {2}", p.Id, p.Name, p.UnitPrice);
            }
            Console.WriteLine("");
            Console.WriteLine("Lista Ordini: ");
            foreach (var o in orderList)
            {
                Console.WriteLine("{0} - {1} - {2}", o.Id, o.ProductId, o.Quantity);
            }


            //Filtro OfType
            var list = new ArrayList();
            list.Add(productList);
            list.Add("Ciao!!");
            list.Add(123);

            var typeQuery =
                from item in list.OfType<List<Product>>()
                select item;

            Console.WriteLine("");
            Console.WriteLine("OfType: ");
            foreach (var item in typeQuery)
            {
                Console.WriteLine("{0}", item);
            }

            //Element
            Console.WriteLine("");
            Console.WriteLine("Elementi: ");
            int[] empty = { };
            var el1 = empty.FirstOrDefault();
            Console.WriteLine(el1);

            var p1 = productList.ElementAt(0).Name;
            Console.WriteLine(p1);


            //Ordinamento prodotti per nome e prezzo
            Console.WriteLine("");
            Console.WriteLine("Ordinamento prodotti per nome e prezzo: ");

           // productList.Add(new Product { Id = 4, Name = "Telefono", UnitPrice = 1000 });

            var orderedList =
                from p in productList
                orderby p.Name ascending, p.UnitPrice descending
                select new { Nome = p.Name, Prezzo = p.UnitPrice };

            var orderedList2 = productList
                .OrderBy(p => p.Name)
                .ThenByDescending(p => p.UnitPrice)
                .Select(p => new { Nome = p.Name, Prezzo = p.UnitPrice })
                .Reverse();

            foreach( var p in orderedList)
            {
                Console.WriteLine("{0} - {1}",p.Nome,p.Prezzo);
            }
            foreach (var p in orderedList2)
            {
                Console.WriteLine("{0} - {1}",p.Nome,p.Prezzo);
            }

            //Quantificatori
            var hasProductWithT = productList.Any(p => p.Name.StartsWith("T"));
            var allProductsWithT = productList.All(p => p.Name.StartsWith("T"));
            Console.WriteLine("Ci sono prodotti che iniziano con la t? {0}", hasProductWithT);
            Console.WriteLine("Tutti i prodotti iniziano con la t? {0}", allProductsWithT);

            //Groupby
            Console.WriteLine();
            Console.WriteLine("Groupby: raggruppiamo order per productId");

            //Query Syntax
            //ragrruppiamo order per ProductId
            var groupByList =
                from o in orderList
                group o by o.ProductId into groupList
                select groupList;

            //Console.WriteLine("");
           // Console.WriteLine("raggruppiamo order per productId");
            foreach (var order in groupByList)
            {
                Console.WriteLine(order.Key);
                foreach(var item in order)
                {

                    Console.WriteLine($"\t {item.ProductId} - {item.Quantity}");
                }
            }


            //Method Syntax
            var groupByList2 =
                orderList
                .GroupBy(o => o.ProductId);
            Console.WriteLine("Method syntax");
            foreach (var order in groupByList2)
            {
                Console.WriteLine(order.Key);
                foreach(var item in order)
                {
                    Console.WriteLine("{0} - {1}", item.ProductId, item.Quantity);
                }
            }



            //Groupby con funzione di aggregazione
            //Raggruppare gli ordini per prodotto e ricavare la somma delle quantità

            var sumQuantityByProduct =
                orderList
                .GroupBy(p => p.ProductId) 
                .Select(lista => new
                {
                    Id = lista.Key,
                    Quantities = lista.Sum(p => p.Quantity)
                });
            Console.WriteLine("");
            Console.WriteLine("Raggrupare gli ordini per prodotto e ricavare la somma delle quantita");
            Console.WriteLine("Groupby con Aggregato: ");
            foreach(var item in sumQuantityByProduct)
            {
                Console.WriteLine("{0} - {1}",item.Id, item.Quantities);
            }

            //Query Syntax
            var sumByProduct2 =
                from o in orderList
                group o by o.ProductId into list3
                select new { Id = list3.Key, Quantities = list3.Sum(x => x.Quantity) };
            Console.WriteLine("Query syntax");
            foreach (var item in sumByProduct2)
            {
                Console.WriteLine("{0} - {1}",item.Id, item.Quantities);
            }


            //Join
            //Recuperiamo i prodotti che hanno ordini
            //Nome - id Ordine - Quantità
            Console.WriteLine("");
            Console.WriteLine("Join: recuperiamo i prodotti che hanno ordini id Ordine-quantita");

            //Method Syntax

            var joinList = productList
                .Join(orderList,
                p => p.Id,
                o => o.ProductId,
                (p, o) => new { Nome = p.Name, OrderId = o.Id, Quantita = o.Quantity });
            Console.WriteLine("Syntax method");
            foreach (var p in joinList)
            {
                Console.WriteLine("{0} - {1} - {2}",p.Nome, p.OrderId,p.Quantita);
            }

            //Query syntax
            var joinedList2 =
                from p in productList
                join o in orderList
                on p.Id equals o.ProductId
                select new
                {
                    Nome = p.Name,
                    OrderId = o.Id,
                    Quantita = o.Quantity
                };
            Console.WriteLine("querry syntax");
            foreach (var p in joinedList2)
            {
                Console.WriteLine("{0} - {1} - {2}",p.Nome, p.OrderId, p.Quantita);
            }

            //GroupJoin
            //Recuperare gli ordini per prodotto e somma quanità
            //Nome Prodotto - Quantità totale

            Console.WriteLine("");
            Console.WriteLine("GroupJoin: recupera gli ordini per prodotto e somma quantita");

            var groupJoinList = productList
                .GroupJoin(orderList,
                p => p.Id,
                o => o.ProductId,
                (p, o) =>
                    new {
                        Prodotto = p.Name,
                        Quantità = o.Sum(o => o.Quantity) });


            Console.WriteLine("Nome prodotto - quantita totale");
            foreach (var item in groupJoinList)
            {
                Console.WriteLine("{0} - {1}",item.Prodotto, item.Quantità);
            }

            var groupJoinList2 =
                from p in productList
                join o in orderList
                on p.Id equals o.ProductId
                into gr
                select new
                {
                    Prodotto = p.Name,
                    Quantità = gr.Sum(o => o.Quantity)
                };

            foreach (var item in groupJoinList2)
            {
                Console.WriteLine("{0} - {1}", item.Prodotto, item.Quantità);
            }

            //Lista Nome Prodotto - Quantità 
            var lista4 =
                from o in orderList
                group o by o.ProductId
                into gr
                select new { ProdottoId = gr.Key, Quantità = gr.Sum(o => o.Quantity) }
                into gr1
                join p in productList
                on gr1.ProdottoId equals p.Id
                select new {p.Name, gr1.Quantità  };

            Console.WriteLine("Podotto-Quantita");
            foreach (var item in lista4 )
            {
                Console.WriteLine("{0} - {1}", item.Name, item.Quantità);
            }



        }

    }
}
