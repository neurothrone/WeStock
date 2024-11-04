namespace WeStock.Blazor.Server.State;

public class AppState
{
    public int CollectionCount { get; private set; }
    public int SectionCount { get; private set; }
    public int ItemCount { get; private set; }

    public Action<int>? OnCollectionCountChanged { get; set; }
    public Action<int>? OnSectionCountChanged { get; set; }
    public Action<int>? OnItemCountChanged { get; set; }
    public Func<Task>? OnDataChanged { get; set; }

    public void IncrementCollectionCount()
    {
        CollectionCount += 1;
        OnCollectionCountChanged?.Invoke(CollectionCount);
    }

    public void DecrementCollectionCount()
    {
        if (CollectionCount < 1)
            return;

        CollectionCount -= 1;
        OnCollectionCountChanged?.Invoke(CollectionCount);
    }

    public void UpdateCollectionCount(int newCount)
    {
        CollectionCount = newCount;
        OnCollectionCountChanged?.Invoke(CollectionCount);
    }

    public void IncrementSectionCount()
    {
        SectionCount += 1;
        OnSectionCountChanged?.Invoke(SectionCount);
        OnDataChanged?.Invoke();
    }

    public void DecrementSectionCount()
    {
        if (SectionCount < 1)
            return;

        SectionCount -= 1;
        OnSectionCountChanged?.Invoke(SectionCount);
        OnDataChanged?.Invoke();
    }

    public void UpdateSectionCount(int newCount)
    {
        SectionCount = newCount;
        OnSectionCountChanged?.Invoke(SectionCount);
        OnDataChanged?.Invoke();
    }

    public void IncrementItemCount()
    {
        ItemCount += 1;
        OnItemCountChanged?.Invoke(ItemCount);
        OnDataChanged?.Invoke();
    }

    public void DecrementItemCount()
    {
        if (ItemCount < 1)
            return;

        ItemCount -= 1;
        OnItemCountChanged?.Invoke(ItemCount);
        OnDataChanged?.Invoke();
    }

    public void UpdateItemCount(int newCount)
    {
        ItemCount = newCount;
        OnItemCountChanged?.Invoke(ItemCount);
        OnDataChanged?.Invoke();
    }
}