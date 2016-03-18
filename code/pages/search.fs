module FsSnip.Pages.Search

open Suave
open Suave.Http.Applicatives
open System
open System.Web
open FsSnip.Utils
open FsSnip.Data

// -------------------------------------------------------------------------------------------------
// Search results page - domain model
// -------------------------------------------------------------------------------------------------

type Results = 
  { Query : string
    Count : int
    Results : Snippet list }

// -------------------------------------------------------------------------------------------------
// Loading search results
// -------------------------------------------------------------------------------------------------

let getResults (query:string) =
  publicSnippets
  |> Seq.filter (fun s -> [ s.Title.ToLowerInvariant() ; s.Comment.ToLowerInvariant() ] |> Seq.exists (fun x -> x.Contains(query.ToLowerInvariant())))

let showResults (query) = delay (fun () -> 
  let decodedQuery = FsSnip.Filters.urlDecode query
  let results = getResults decodedQuery |> Seq.toList
  DotLiquid.page "search.html" { Query = decodedQuery
                                 Results = results
                                 Count = (List.length results) })

let webPart = pathScan "/search/%s" showResults                                 