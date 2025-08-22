namespace GenericsWithDIContainer;
internal class Consumer(GenericService<int> service)
{
    public void Consume()
    {

        service.WriteGenericType();
    }
}
