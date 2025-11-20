// NoteData 객체를 생성하는 기본 팩토리 구현
public class NoteDataFactory : INoteDataFactory
{
    private readonly INoteTypeResolver typeResolver;

    public NoteDataFactory(INoteTypeResolver resolver)
    {
        this.typeResolver = resolver;
    }

    public NoteData Create(int noteLane, float noteStart, int noteType, string noteEnd)
    {
        // 라인 0 ~ 3
        int lineIndex = GetLineIndex(noteLane);
        // 노트 타입 롱, 숏
        Note type = typeResolver.ResolveType(noteType, noteEnd);

        float endTime = noteStart;

        // 롱노트일 경우 end에서 endTime 추출
        if (type == Note.Long && !string.IsNullOrEmpty(noteEnd))
        {
            string[] extraParts = noteEnd.Split(':');
            if (extraParts.Length > 0 && float.TryParse(extraParts[0], out float parsedEnd))
            {
                endTime = parsedEnd;
            }
        }

        return new NoteData(lineIndex, noteStart, type, endTime);
    }

    // x 좌표를 기반으로 라인 인덱스를 계산
    private int GetLineIndex(int x)
    {
        if (x < 128) return 0;
        if (x < 256) return 1;
        if (x < 384) return 2;
        return 3;
    }
}
