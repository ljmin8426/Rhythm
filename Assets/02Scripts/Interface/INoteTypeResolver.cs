// type과 extras 정보를 해석하여 NoteType을 반환하는 인터페이스
// SRP 타입 판별만 책임지고 있음
public interface INoteTypeResolver
{
    Note ResolveType(int rawType, string extras);
}
