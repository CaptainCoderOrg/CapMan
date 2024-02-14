namespace CapMan.Tests;

public class BoundingBox_should_
{

    [Theory]
    [MemberData(nameof(OverlappingBoxes))]
    public void intersect_when_overlapping(BoundingBox a, BoundingBox b)
    {
        a.IntersectsWith(b).ShouldBeTrue();
        b.IntersectsWith(a).ShouldBeTrue();
    }
    public static IEnumerable<object[]> OverlappingBoxes => [
        [new BoundingBox(0, 0, 2, 2), new BoundingBox(0, 0, 2, 2)],
        [new BoundingBox(0, 0, 2, 2), new BoundingBox(1, 1, 2, 2)],
        [new BoundingBox(0, 0, 2, 2), new BoundingBox(-1, -1, 2, 2)],
        [new BoundingBox(0, 0, 2, 2), new BoundingBox(-1, 1, 2, 2)],
        [new BoundingBox(0, 0, 2, 2), new BoundingBox(1, -1, 2, 2)]
    ];

    [Theory]
    [MemberData(nameof(NonOverlappingBoxes))]
    public void does_not_intersect(BoundingBox a, BoundingBox b)
    {
        a.IntersectsWith(b).ShouldBeTrue();
        b.IntersectsWith(a).ShouldBeTrue();
    }
    public static IEnumerable<object[]> NonOverlappingBoxes => [
        [new BoundingBox(0, 0, 2, 2), new BoundingBox(-2, -2, 2, 2)],
        [new BoundingBox(0, 0, 2, 2), new BoundingBox(0, -2, 2, 2)],
        [new BoundingBox(0, 0, 2, 2), new BoundingBox(2, -2, 2, 2)],

        [new BoundingBox(0, 0, 2, 2), new BoundingBox(-2, 0, 2, 2)],
        [new BoundingBox(0, 0, 2, 2), new BoundingBox(2, 0, 2, 2)],

        [new BoundingBox(0, 0, 2, 2), new BoundingBox(-2, 2, 2, 2)],
        [new BoundingBox(0, 0, 2, 2), new BoundingBox(0, 2, 2, 2)],
        [new BoundingBox(0, 0, 2, 2), new BoundingBox(-2, 2, 2, 2)],
    ];
}