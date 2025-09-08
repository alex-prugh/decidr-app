export interface Card {
  id: number;
  title: string;
  description: string;
  imageUrl?: string | null;
  isLiked?: boolean;
  isDisliked?: boolean;
}