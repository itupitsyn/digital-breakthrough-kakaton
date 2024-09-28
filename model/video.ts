export type Video = {
  video_id: number;
  title: string;
  description?: string;
  category: string;
  v_pub_datetime: number;
  preview?: string;
  state?: "like" | "dislike";
  v_likes: number;
  v_dislikes: number;
  v_duration: number;
};
