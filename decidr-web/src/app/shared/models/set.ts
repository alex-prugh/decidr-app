import { Card } from './card';

export interface Set {
  id: number;
  name: string;
  imageUrl?: string | null;
  hasVoted: boolean;
  cards: Card[];
}