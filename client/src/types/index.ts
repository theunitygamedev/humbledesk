export enum AudienceType {
  Employee = 1,
  BoardDirector = 2,
}

export enum QuestionSetStatus {
  Draft = 1,
  Preview = 2,
  Published = 3,
  Archived = 4,
}

export enum QuestionType {
  YesNo = 1,
  ShortText = 2,
  LongText = 3,
  YesNoWithConditionalText = 4,
  SingleSelect = 5,
  MultiSelect = 6,
  Date = 7,
  Number = 8,
  FileUpload = 9,
  Attestation = 10,
}

export interface QuestionSetDto {
  id: string;
  name: string;
  audienceType: AudienceType;
  version: number;
  status: QuestionSetStatus;
  createdBy: string;
  createdAt: string;
  sections: SectionDto[];
}

export interface SectionDto {
  id: string;
  order: number;
  title: string;
  description?: string;
  questions: QuestionDto[];
}

export interface QuestionDto {
  id: string;
  order: number;
  textHtml: string;
  helpHtml?: string;
  type: QuestionType;
  required: boolean;
  constraintsJson?: string;
  tags?: string;
  options: OptionDto[];
}

export interface OptionDto {
  id: string;
  value: string;
  label: string;
  order: number;
}
