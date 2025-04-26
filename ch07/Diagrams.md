# Diagrams

info

## Linked List

classDiagram

  class LinkedList~T~ {
    +First: LinkedListNode~T~
    +Last: LinkedListNode~T~
    +Count : int
    +AddFirst(T value)
    +AddLast(T value)
    +AddAfter(LinkedListNode~T~ node, T value)
    +AddBefore(LinkedListNode~T~ node, T value)
    +Remove(LinkedListNode~T~ node)
    +RemoveFirst()
    +RemoveLast()
  }

  class LinkedListNode~T~ {
    +Value: T
    +Next: LinkedListNode~T~
    +Previous: LinkedListNode~T~
    +ValueRef: ref T
  }

  class SomeValue {
    <<struct>>
    +X : long
    +Y : long
  }

# Diagrams  

## Object Diagrams


### Array of Structs

```mermaid
objectDiagram object ArrayOfSomeValue { elements: SomeValue[5] }
object SomeValue1 { X: long Y: long }
object SomeValue2 { X: long Y: long }
object SomeValue3 { X: long Y: long }
object SomeValue4 { X: long Y: long }
object SomeValue5 { X: long Y: long }
ArrayOfSomeValue : elements --> SomeValue1 ArrayOfSomeValue : elements --> SomeValue2 ArrayOfSomeValue : elements --> SomeValue3 ArrayOfSomeValue : elements --> SomeValue4 ArrayOfSomeValue : elements --> SomeValue5

```

