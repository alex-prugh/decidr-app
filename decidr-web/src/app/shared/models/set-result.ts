export interface SetResult {
  id: number;
  name?: string | null;
  cardSummaries: CardSummary[];
}

export interface CardSummary {
  title?: string | null;
  imageUrl?: string | null;
  likes: number;
  dislikes: number;
  isSuggested: boolean;
}