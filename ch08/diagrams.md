# IEnumerable and IEnumerator

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
