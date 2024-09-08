using LINQ_MySQL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LINQ_MySQL {
    internal class Program {

        static void Print<T>(string message, IEnumerable<T> collection) {
                        
            Console.WriteLine(message);
            foreach (T obj in collection) {
                Console.WriteLine(obj);
            }            
        }

        static void Main(string[] args) {

            Category c1 = new Category() { Id = 1, Name = "Tools", Tier = 2 };
            Category c2 = new Category() { Id = 2, Name = "Computers", Tier = 1 };
            Category c3 = new Category() { Id = 3, Name = "Electronics", Tier = 1 };

            List<Product> prod = new List<Product>() {
                new Product() { Id = 1, Name = "Computer", Price = 1100.0, Category = c2 },
                new Product() { Id = 2, Name = "Hammer", Price = 90.0, Category = c1 },
                new Product() { Id = 3, Name = "TV", Price = 1700.0, Category = c3 },
                new Product() { Id = 4, Name = "Notebook", Price = 1300.0, Category = c2 },
                new Product() { Id = 5, Name = "Saw", Price = 80.0, Category = c1 },
                new Product() { Id = 6, Name = "Tablet", Price = 700.0, Category = c2 },
                new Product() { Id = 7, Name = "Camera", Price = 700.0, Category = c3 },
                new Product() { Id = 8, Name = "Printer", Price = 350.0, Category = c3 },
                new Product() { Id = 9, Name = "MacBook", Price = 1800.0, Category = c2 },
                new Product() { Id = 10, Name = "Sound Bar", Price = 700.0, Category = c3 },
                new Product() { Id = 11, Name = "Level", Price = 70.0, Category = c1 }
            };


            Console.WriteLine("Produtos da tier 1 com preços abaixo de R$ 900,00");
            /*                            
             *      O comando Where filtra produtos da base de dados, neste caso, pegamos todos os produtos que estejam na categoria
             *  01, e que tenham preço abaixo de R$ 900,00.                        
             */

            // Linq
            var resLinq01 = prod.Where(p => p.Category.Tier == 1 && p.Price < 900.0);
            Print("\nCódigo LINQ: ", resLinq01);

            // Banco de dados
            var bancoDados01 = 
                from p in prod
                where p.Category.Tier == 1 && p.Price < 900.0
                select p;
            Print("\nCódigo MySQL:", bancoDados01);

            Console.WriteLine("\n=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-\n");




            Console.WriteLine("Lista de produtos da categoria 'Tools'");
            /* 
             *      O comando Where filtra produtos da base de dados, neste caso, pegamos todos os produtos da categoria Tools, o comando
             *  Select, seleciona da base de dados um valor, neste caso, pegamos somente os nomes dos produtos da categoria ferramenta
             */
            
            // Código LINQ
            var resLinq02 = prod.Where(p => p.Category.Name == "Tools").Select(p => p.Name);
            Print("\nCódigo LINQ:",resLinq02);

            // Código MySQL
            var bancoDados02 = 
                from p in prod
                where p.Category.Name == "Tools"
                select p.Name;
            Print("\nCódigo MySQL:", bancoDados02);

            Console.WriteLine("\n=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-\n");




            Console.WriteLine("Produtos com a letra C.\nRetorna o nome, preço e categoria.");
            /* 
             *      O comando Where filtra os produtos da base de dados, neste caso, estamos acessando o primeiro caracter no nome do
             *  produto e pegando os produtos com a letra C. O comando Select está criando um objeto anonimo, pegando da base de dados,
             *  somente o nome, preço e categoria do produto. Observamos que estamos atribuíndo uma variavel "CategoryName" para
             *  armazenar o nome da categoria, isso porque, tanto o nome da categoria na classe Category, e o nome do produto da 
             *  classe Product, possui o mesmo nome de atributo, causando conflito na linha de código, por isso criamos uma variavel
             *  para receber o nome da categoria, eliminando assim, o conflito dos atributos iguais.
             */
            
            var resLinq03 = prod.Where(p => p.Name[0] == 'C').Select(p => new { p.Name, p.Price, Categoria = p.Category.Name });
            Print("\nCódigo LINQ: ", resLinq03);


            var bancoDados03 = 
                from p in prod
                where p.Name[0] == 'C'
                select new {p.Name, p.Price, Categoria = p.Category.Name};
            Print("\nCódigo MYSQL: ", bancoDados03);

            Console.WriteLine("\n=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-\n");




            Console.WriteLine("Produtos da tier 1, ordenados por preço e subsequentemente por nome");
            /*
             *      O comando Where filtra os produtos da base de dados, neste caso, filstramos todos os produtos da tier 1, O comando 
             *  OrderBy irá ordenar os produtos filtrados em ordem de preço conforme argumento estabelecido, já o comando ThenBy é
             *  caracterizado como segundo modo de ordenação, conforme arugmento, ele ordenará por nome.
             *  Ou seja, está linha de código filtra os produtos de tier 1, ordena eles em ordem de preço e caso haja mais de um produto
             *  com o mesmo valor, será ordenado utilizando os nomes dos mesmos.
             */
            var resLinq04 = prod.Where(p => p.Category.Tier == 1).OrderBy(p => p.Price).ThenBy(p => p.Name);
            Print("\nCódigo LINQ: ", resLinq04);

            var bancoDados04 =
                from p in prod
                where p.Category.Tier == 1
                orderby p.Price, p.Name
                select p;
            Print("\nCódigo MYSQL: ", bancoDados04);

            Console.WriteLine("\n=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-\n");




            Console.WriteLine("Produtos da tier 1, ordenados por preço e subsequentemente por nome.\n" +
                "No caso abaixo, vamos pula os dois primeiros itens da lista e imprimir ao usuário somente\n" +
                "os quatro próximos itens da lista, ignorando seu final");
            /*
             *      Nas linhas abaixo reutilizamos a variavel trabalha no r4, esta filtra os produtos da tier 1, ordenados por preço, caso
             *  haja mais de um valor com o mesmo preço, ordenará por nome. Está lista já filtrada recebe os metodos skip e take, tetodos
             *  estes com uso mais comum para paginação, o comando Skip(2) irá pular os dois primeiros elementos e o comando take(4)
             *  pegará os proximos quatro elementos da lista. 
             */
            var resLinq05 = resLinq04.Skip(2).Take(4);
            Print("Código LINQ: ", resLinq05);

            var bancoDados05 = 
                (from p in bancoDados04
                select p).Skip(2).Take(4);



            Console.WriteLine("Primeiro elemento da lista ou um valor nulo");
            /*
             *          Nas linhas abaixo testaremos o metodo que retorna o primeiro elemento de uma lista, o metodo .First(). Existe
             *  um metodo alternativo ao .First() que é o .FirstOrDefault() que realiza a mesma função, retorna o primeiro elemento da
             *  lista de produtos, a diferença é que, o método .First(), caso seja utilizado em uma lista vazia, ou usado pós .Where()
             *  com alguma condição que não retorno nenhum resultado, o programa sofrerá uma excessão, sendo necessário o tratamento
             *  pelo programador, porém, o método .FirtOrDefault(), retornará um valor nulo em casos similares, não acontecendo
             *  excessões no programa se tornando assim, uma alternativa mais interessante a ser usada.
             *  
             *      Vale resaltar que todos as explicações acima tem igual fundamentos para os métodos .Last() e .LastOrDefault().
             *  Estes dois métodos retornam o último elemento da lista e não será testado abaxio já que possuem comportamentos iguais
             *  ao métodos testados
             */

            Console.WriteLine("\nCódigo LINQ: ");
            var resLinq06 = prod.FirstOrDefault();
            Console.WriteLine("Primeiro elemento da lista ou um valor nulo: " + resLinq06);
            var resLinq07 = prod.Where(p => p.Price > 30000.0).FirstOrDefault();
            Console.WriteLine("Primeiro elemento da lista ou um valor nulo: " + resLinq07);
            Console.WriteLine();

            Console.WriteLine("\nCódigo MySQL: ");
            var bancoDados06 = 
                (from p in prod
                 select p).FirstOrDefault();
            
            Console.WriteLine();
            var bancoDados07 = 
                (from p in prod
                 where p.Price > 30000.0
                 select p).FirstOrDefault();
            Console.WriteLine();




            /*
             *      Agora estudaremos os métodos .Single() e .SingleOrDefault(), ele possui o mesmo fundamento dos métodos estudados
             *  acima, neste caso, ele retorna um valor ou um valor nulo, e retorna somente um valor, seu uso é para casos onde se
             *  pesquisa o produto por sua ID, sendo elas primary key, ou seja, é um valor único por produto não podendo ser repetido
             *  em outro produto, o método .SingleOrDefault() retorná um elemento de ID, caso não exista nenhum produto com esta Id,
             *  o método retornará um valor nulo, porémcaso exista mais de um produto com amesma ID, o programa lançará uma excessão,
             *  o método retornará um valor null. Obviamente se o valor pesquisado é único, visando ter apenas uma ID, não podemos
             *  utilizar condições estilo (p => p.Id >= 5)
             */
            var resLinq08 = prod.Where(p => p.Id == 3).SingleOrDefault();
            Console.WriteLine("Produto único ou um valor nulo" + resLinq08);
            var resLinq09 = prod.Where(p => p.Id == 30).SingleOrDefault();
            Console.WriteLine("Produto único ou um valor nulo" + resLinq09);
            Console.WriteLine();

            // Testando os métodos matemáticos


            // O método Max retorna o maior valor da base de dados
            var resLinq10 = prod.Max(p => p.Price);
            Console.WriteLine("Produto de maior preço" + resLinq10);


            // O método Min retorna o menor valor da base de dados
            var resLinq11 = prod.Min(p => p.Price);
            Console.WriteLine("Produto de menor preço" + resLinq11);


            /* Abaixo filtramos os produto da categoria 1 com o método .Where, e com o método .Sum() somamos todos os produtos
             *  filtrados
            */
            var resLinq12 = prod.Where(p => p.Category.Id == 1).Sum(p => p.Price);
            Console.WriteLine("Soma de todos os produtos da tier 1" + resLinq12);


            /* Abaixo utilizamos o método Where para filtrar todos os itens da categoria por sua ID, e subsequentemente com o método
             * .Avarage, pegamos a média do preço desses produtos, caso não exista um valor na coleção o programa retornará um erro
             * porque ela tentará divir um valor por zero, o que não é permitido em programação.
             */
            var resLinq13 = prod.Where(p => p.Category.Id == 1).Average(p => p.Price);
            Console.WriteLine("Média dos preços dos produtos de tier 1" + resLinq13);


            /*
             *      Como no exemplo acima pode retornar um erro no programa caso a filtragem do .Where() não retorne um valor, podemos
             * utilizar o método DefaultIfEmpty, que retornará null caso esta filtragem não retorne nenhum resultado, não lançando
             * excessões no programa.
             * 
             *      Exemplificando o código, .Where realiza uma filtragem na base de dados de product, utilizando a Id da Categoria 5
             * valor este inexistente no banco de dados.
             * O método .Select está orientando que essa pesquisa retorne o preço dos produtos.
             * O médoto DefaultIfEmpty, faz com que retorne o valor contido em seu argumento, neste caso, 0.0, caso o Where e/ou o Select
             * retorne um valor vazio.
             *      O método Avarage retorna a média do preços dos produtos              *
             */
            var resLinq14 = prod.Where(p => p.Category.Id == 5).Select(p => p.Price).DefaultIfEmpty(0.0).Average();
            Console.WriteLine("Category 5 Average prices: " + resLinq14);


            /*
             *      O código abaixo filtra os elementos do id da categoria com o método Where, utilizada o .Select para selecionar
             *  o preço do produto, a seguir, utilizamos o método .Aggregate(), que serve para realizar contas dos preços dos produtos, 
             *  esta conta já que ela é feita manualmente diretamente no arugmento do método.
             *      Os argumentos são no padrão lambda e recebe dois argumento (x, y), e a seguir a conta que deseja realizar.
             *      
             *      Por exemplo... 
             *          .Aggegrate((x, y) => x + y); -> Retorna a soma dos produtos filtrados
             *          
             *      Caso a filtragem desses produtos estejam vazia será levantada uma excessão, o que pode ser tratada com a sobre carga
             *  do método em que coloca um valor defalt antes do primeiro parenteses (x, y), este número precisa ser double, um valor int
             *  não será aceito pelo compilador.
             *  
             *      Por exemplo...
             *          .Aggegrate(0.0(x, y) => x + y);
             *          
             *          
             */

            var resLinq15 = prod.Where(p => p.Category.Id == 1).Select(p => p.Price).Aggregate(0.0, (x, y) => x + y);
            Console.WriteLine("Category 1 aggregate sum: " + resLinq15);

            Console.WriteLine();


            /*
             *      Abaixo temos um exemplo da criaçao de um agrupamento de produtos
             * 
             *      Iniciamos o código com o método .GroupBy, agrupar por, utilizando a categoria do produto para realizar o agrupamento
             *      
             *      O tipo de retorno dete método será de um IGrouping, possuíndo chave e valor, similar a um dicionário, o foreach
             *  percorre a lista de produtos, fazmos um foreach dentro do outro onde, o primeiro imprime o nome da chave e o segundo 
             *  for imprime o .ToString() do produto.
             *  
             *      O grup.Key.Name imprime o nome da categoria, logo, ele imprime esta categoria na tela, entra no segundo foreach
             *  e imprime todos os produtos desta categoria na tela, ao terminar, retorna ao primeiro foreach para imprimir o nome
             *  da próxima categoria, entrando novamente no segundo for para imprirmir o nome de todos os produtos contidos nesta
             *  categoria.
             *  
             *      O programa repete mais uma vez apenas pois temos somente três categoria de produtos na base de dados.  
             */

            var resLinq16 = prod.GroupBy(p => p.Category);
            foreach (IGrouping<Category, Product> group in resLinq16) {
                Console.WriteLine("Category " + group.Key.Name + ":");
                foreach (Product p in group) {
                    Console.WriteLine(p);
                }
                Console.WriteLine();
            }
        }
    }
}