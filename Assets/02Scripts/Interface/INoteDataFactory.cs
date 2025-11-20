// 파싱된 정보(x, time, type, extras)를 NoteData 객체로 만드는 인터페이스
public interface INoteDataFactory
{
    NoteData Create(int x, float startTime, int rawType, string extras);
}