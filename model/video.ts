export type Video = {
  id: string;
  name: string;
  description?: string;
  preview: string;
  date: number;
  state: "like" | "dislike" | null;
};
