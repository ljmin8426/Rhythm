// 기본 구현: 비트마스크 기반으로 롱노트를 판별
public class DefaultNoteTypeResolver : INoteTypeResolver
{
    public Note ResolveType(int rawType, string extras)
    {
        // type 값에 128 비트가 켜져있으면 롱노트
        if ((rawType & 128) > 0)
            return Note.Long;

        return Note.Short;
    }
}