open System
open System.IO
open Newtonsoft.Json

let JSONPath = "data.json"

// For more information see https://aka.ms/fsharp-console-apps
type Todo(title:string, isFinished:bool) =
    member val Title = title with get, set
    member val IsFinished = isFinished with get, set
    member val ID = Guid.NewGuid() with get
    member this.IsSame(other:Todo) = this.ID = other.ID

    static member Deserialize json = 
        JsonConvert.DeserializeObject<Todo list>(json)

let mutable todoList = [] : Todo list

if File.Exists JSONPath then
    todoList <- Todo.Deserialize (File.ReadAllText JSONPath)

while true do
    Console.Clear()
    printfn "What would you like to do?"
    printfn "1. Add Todo"
    printfn "2. Remove Todo"
    printfn "3. Mark Todo as complete"
    printfn "4. Remove finished"
    printfn "5. List Todos"
    printfn "Q. Exit"

    let input =  Console.ReadKey(true).KeyChar
    Console.Clear()


    match input with
    | '1' ->
        printf "Enter the title of the new todo:"
        let title = Console.ReadLine()
        todoList <- todoList @ [new Todo(title, false)]
    | '2' -> 
        printfn("Select todo to remove")
        for i in 0..todoList.Length-1 do
            printfn("Select todo to remove")
            for i in 0..todoList.Length-1 do
                printfn(@$"{i}. {todoList.[i].Title}")
        let index = Console.ReadLine() |> int
        todoList <- todoList 
                    |> List.mapi (fun i x -> (i, x)) 
                    |> List.filter(fun (i, x) -> i <> index)
                    |> List.map snd
    | '3' ->
        printfn("Select todo to mark as complete")
        for i in 0..todoList.Length-1 do
            printfn(@$"{i}. {todoList.[i].Title}")
        let index = Console.ReadLine() |> int
        todoList <- todoList 
                    |> List.mapi (fun i x -> (i, x)) 
                    |> List.map (fun (i, x) -> if i = index then new Todo (x.Title, true) else x)
    | '4' ->
        todoList <- todoList |> List.filter (fun x -> x.IsFinished = false)
    | '5' ->
        for i in 0..todoList.Length-1 do
            printfn $"""{i}. {todoList.[i].Title} - {if todoList.[i].IsFinished then "Finished" else "Not Finished"}"""
        Console.ReadLine() |> ignore
    | 'Q' | 'q' ->
        File.WriteAllText(JSONPath, JsonConvert.SerializeObject(todoList))
        Environment.Exit(0)
    | _ ->
        printfn "Invalid option"



