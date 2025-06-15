# Diagrams

## IEnumerable and IEnumerator

```mermaid
classDiagram
    class IEnumerable~T~ {
        +GetEnumerator() IEnumerator~T~
    }
    class IEnumerator~T~ {
        +Current : T
        +MoveNext() bool
        +Reset()
        +Dispose()
    }
    IEnumerable~T~ "1" --> "1" IEnumerator~T~ : GetEnumerator()
```

# Diagrams

## Inner join

```mermaid
flowchart TD
    subgraph Racers
        R1["Racer: Year, Name"]
    end
    subgraph Teams
        T1["Team: Year, Name"]
    end
    R1 -- "Year == Year" --- T1
    R1 & T1 --> IJ["Result: Year, Champion, Constructor"]
```

InnerJoin matches only records with the same year in both sources.

## Group join

```mermaid
flowchart TD
    subgraph Champions
        C1["Champion: FirstName, LastName, Wins"]
    end
    subgraph Results
        R2["Result: Year, Position, FirstName, LastName"]
    end
    C1 -- "(FirstName, LastName) == (FirstName, LastName)" --- R2
    C1 & R2 --> GJ["Result: FirstName, LastName, Wins, Results[]"]
```

GroupJoin groups all results for each champion by name.

## Left outer join

```mermaid
flowchart TD
    subgraph Racers
        R3["Racer: Year, Name"]
    end
    subgraph Teams
        T2["Team: Year, Name"]
    end
    R3 -- "Year == Year (may not match)" --- T2
    R3 & T2 --> LOJ["Result: Year, Champion, Constructor (or 'no constructor championship')"]
```

LeftOuterJoin includes all racers, even if no matching team exists for a year.
