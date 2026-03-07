public interface IWorldSegmentPoolService
{
    RoadSegmentView Get(RoadSegmentView prefab);
    void Return(RoadSegmentView segment);
}