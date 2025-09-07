import { Card } from './card';

export interface Set {
  id: number;
  name: string;
  imageUrl?: string | null;
  isUnread: boolean;
  cards: Card[];
}